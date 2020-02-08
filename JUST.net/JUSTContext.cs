﻿using JUST.net.Selectables;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JUST
{
    public enum EvaluationMode
    {
        FallbackToDefault,
        Strict
    }

    public class JUSTContext
    {
        private Dictionary<string, MethodInfo> _customFunctions = new Dictionary<string, MethodInfo>();
        private int _defaultDecimalPlaces = 28;

        internal JToken Input;

        public EvaluationMode EvaluationMode = EvaluationMode.FallbackToDefault;

        public int DefaultDecimalPlaces
        {
            get { return _defaultDecimalPlaces; }
            set
            {
                if (value < 0 || value > 28) { throw new ArgumentException("Value must be between 1 and 28"); }
                _defaultDecimalPlaces = value;
            }
        }

        public JUSTContext() { }

        internal JUSTContext(string inputJson)
        {
            Input = JToken.Parse(inputJson);
        }

        public void RegisterCustomFunction(string assemblyName, string namespc, string methodName, string methodAlias = null)
        {
            var methodInfo = ReflectionHelper.SearchCustomFunction(assemblyName, namespc, methodName);
            if (methodInfo == null)
            {
                throw new Exception("Unable to find specified method!");
            }

            _customFunctions.Add(methodAlias ?? methodName, methodInfo);
        }

        public void UnregisterCustomFunction(string name)
        {
            _customFunctions.Remove(name);
        }

        public void ClearCustomFunctionRegistrations()
        {
            _customFunctions.Clear();
        }

        internal MethodInfo GetCustomMethod(string key)
        {
            if (!_customFunctions.TryGetValue(key, out var result))
            {
                throw new Exception($"Custom function {key} is not registered!");
            }
            return result;
        }

        internal bool IsRegisteredCustomFunction(string name)
        {
            return _customFunctions.ContainsKey(name);
        }

        internal T Resolve<T>(JToken token) where T: ISelectableToken
        {
            T instance = Activator.CreateInstance<T>();
            instance.Token = token;
            return instance;
        }
    }
}
