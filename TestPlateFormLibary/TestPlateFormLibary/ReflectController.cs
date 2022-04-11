using System;
using System.Collections.Generic;
using TestPlateFormLibary.Struct;

namespace TestPlateFormLibary
{
    public class ReflectController
    {

        #region Property

       /// <summary>
       /// 缓存每个dll反射后的基础数据
       /// </summary>
        public Cache<string, Reflectpara> dllreflectMsg { get; set; }
        /// <summary>
        /// 统一函数接口的委托
        /// </summary>
        /// <param name="config">每个dll函数方法所需的参数，以'\r\n'分隔</param>
        /// <param name="result">返回函数执行结果true/false</param>
        /// <returns></returns>
        private delegate bool methodInvokeDelegate(List<string> config, out HashSet<string> result);
        /// <summary>
        /// 缓存每个dll的Delegate.CreateDelegate对象,以提高反射效率
        /// </summary>
        private readonly Dictionary<string, methodInvokeDelegate> dicmethodInvokeDelegate = new Dictionary<string, methodInvokeDelegate>(5);

        #endregion

        /// <summary>
        ///  此方法利用反射作为整个平台的底层实现
        /// </summary>
        /// <param name="testPara">参数配置界面中所需参数所组成的结构体</param>
        /// <param name="hashSetMsg">函数内部吐露出的信息</param>
        /// <returns></returns>
        private bool Test_Run_RunByNameTest(TestPara testPara,out HashSet<string> hashSetMsg)
        {
            //此为底层Driver实现
            Type type;
            hashSetMsg = null;
            bool result = false;
            object classInstance;
            string methodName, classNameKey;
 
            try
            {
                List<string> config = new List<string>(testPara.config.TrimStart().TrimEnd().Split(new[] { "\r\n" }, StringSplitOptions.None));
                methodName = testPara.methodName;
                classNameKey = testPara.classNameKey;
                if (dllreflectMsg == null & classNameKey == null & methodName == null & config == null) return false;
                type = dllreflectMsg.Get(classNameKey).type;
                classInstance = dllreflectMsg.Get(classNameKey).classInstance;
                if (type == null & classInstance == null) return false;
                methodInvokeDelegate methodInvokeDelegate = null;
                if (!dicmethodInvokeDelegate.TryGetValue(classNameKey, out methodInvokeDelegate) && dicmethodInvokeDelegate.Count == 0)
                {
                    methodInvokeDelegate = (methodInvokeDelegate)Delegate.CreateDelegate(typeof(methodInvokeDelegate), null, type.GetMethod(methodName));
                    dicmethodInvokeDelegate.Add(classNameKey, methodInvokeDelegate);
                }
                result=dicmethodInvokeDelegate[classNameKey](config, out hashSetMsg);
            }
            finally
            {
                type = null;
                classInstance = null;
                methodName = classNameKey = String.Empty;
            }
            return result;
        }
    }
}
