﻿using Camstar.Exceptions;
using System;
using System.Reflection;

namespace Camstar.Util
{
    public class ServiceObject
    {
        private object mServiceObject = null;
        private Type mServiceType = null;
        private bool mReplaceValue = false;

        public object Object
        {
            get => this.mServiceObject;
            set
            {
                this.mServiceObject = value != null ? value : throw new ArgumentNullException(nameof(value));
                this.mServiceType = this.mServiceObject.GetType();
            }
        }

        public ServiceObject(object serviceObject) => this.Object = serviceObject;

        public void SetValue(string fieldExpr, object fieldValue)
        {
            FieldInfo fieldInfo;
            object parentObj;
            if (this.GetFieldInfo(fieldExpr, out fieldInfo, out parentObj))
            {
                if (fieldValue == null || this.IsRightFieldType(fieldExpr, fieldValue.GetType()))
                {
                    if (fieldInfo.FieldType.IsGenericType && fieldValue != null)
                        fieldValue = fieldInfo.FieldType.GetConstructor(fieldInfo.FieldType.GetGenericArguments()).Invoke(new object[1]
                        {
              fieldValue
                        });
                    if (this.ReplaceValue)
                        fieldInfo.SetValue(parentObj, fieldValue);
                    else if (fieldInfo.GetValue(parentObj) == null)
                        fieldInfo.SetValue(parentObj, fieldValue);
                }
                else
                    throw new CamstarException("FieldTypeMismatched", new string[4]
                    {
            fieldValue.GetType().Name,
            fieldExpr,
            fieldInfo.FieldType.Name,
            this.mServiceType.Name
                    });
            }
            else
                throw new CamstarException("InvalidFieldExpression", new string[2]
                {
          fieldExpr,
          this.mServiceType.Name
                });
        }

        public object GetValue(string fieldExpr)
        {
            object obj = null;
            FieldInfo fieldInfo;
            object parentObj;
            if (this.GetFieldInfo(fieldExpr, out fieldInfo, out parentObj))
                obj = fieldInfo.GetValue(parentObj);
            return obj;
        }

        public Type GetFieldType(string fieldExpr)
        {
            Type type = null;
            FieldInfo fieldInfo;
            if (this.GetFieldInfo(fieldExpr, out fieldInfo, out object _))
                type = fieldInfo.FieldType;
            return type;
        }

        public bool IsRightFieldType(string fieldExpr, Type fieldType)
        {
            bool flag = false;
            if (fieldType == null)
            {
                flag = true;
            }
            else
            {
                Type fieldType1 = this.GetFieldType(fieldExpr);
                if (fieldType1 != null)
                    flag = this.IsMatchedType(fieldType1, fieldType);
            }
            return flag;
        }

        public bool IsFieldExist(string fieldExpr) => this.GetFieldInfo(fieldExpr, out FieldInfo _, out object _);

        public bool ReplaceValue
        {
            set => this.mReplaceValue = value;
            get => this.mReplaceValue;
        }

        public bool GetFieldInfo(string fieldExpr, out FieldInfo fieldInfo, out object parentObj)
        {
            if (fieldExpr == null)
                throw new ArgumentNullException(nameof(fieldExpr));
            parentObj = this.mServiceObject;
            object obj = this.mServiceObject;
            fieldInfo = null;
            char[] chArray1 = new char[1] { '.' };
            string[] strArray1 = fieldExpr.Split(chArray1);
            Type type1 = this.mServiceType;
            string empty = string.Empty;
            foreach (string str in strArray1)
            {
                char[] chArray2 = new char[1] { ':' };
                string[] strArray2 = str.Split(chArray2);
                string name = strArray2[0];
                if (obj == null && type1 != null && !type1.IsArray)
                {
                    if (string.IsNullOrEmpty(empty))
                    {
                        obj = type1.Assembly.CreateInstance(type1.FullName);
                    }
                    else
                    {
                        type1 = type1.Assembly.GetType(type1.Namespace + "." + empty, false);
                        if (type1 != null)
                            obj = type1.Assembly.CreateInstance(type1.FullName);
                        else
                            break;
                    }
                    fieldInfo.SetValue(parentObj, obj);
                }
                if (type1 != null && type1.IsArray)
                {
                    if (obj != null)
                    {
                        var array = obj as Array;
                        if (array.Length > 0)
                        {
                            obj = array.GetValue(0);
                            type1 = obj.GetType();
                        }
                    }
                    else
                    {
                        Array instance = Array.CreateInstance(type1.GetElementType(), 1);
                        fieldInfo.SetValue(parentObj, instance);
                        if (string.IsNullOrEmpty(empty))
                        {
                            obj = type1.Assembly.CreateInstance(type1.GetElementType().FullName);
                        }
                        else
                        {
                            Type type2 = type1.Assembly.GetType(type1.Namespace + "." + empty, false);
                            if (type2 != null)
                                obj = type2.Assembly.CreateInstance(type2.FullName);
                            else
                                break;
                        }
                        type1 = obj.GetType();
                        instance.SetValue(obj, 0);
                    }
                }
                if (strArray2.Length > 1)
                {
                    empty = strArray2[1];
                    if (type1 != null && type1.Name.EndsWith("_Info"))
                        empty += "_Info";
                }
                else
                    empty = string.Empty;
                parentObj = obj;
                fieldInfo = type1.GetField(name);
                if (fieldInfo != (FieldInfo)null)
                {
                    obj = fieldInfo.GetValue(parentObj);
                    type1 = obj == null ? fieldInfo.FieldType : obj.GetType();
                }
            }
            return fieldInfo != (FieldInfo)null && parentObj != null;
        }

        protected bool IsMatchedType(Type targetType, Type fieldType)
        {
            bool flag = false;
            if (targetType.IsArray && fieldType.IsArray)
            {
                targetType = targetType.GetElementType();
                fieldType = fieldType.GetElementType();
            }
            for (; !flag && fieldType != null; fieldType = fieldType.BaseType)
            {
                if (fieldType.IsGenericType)
                    fieldType = fieldType.GetGenericArguments()[0];
                if (targetType.IsGenericType)
                    targetType = targetType.GetGenericArguments()[0];
                flag = targetType.Name == fieldType.Name;
            }
            return flag;
        }
    }
}