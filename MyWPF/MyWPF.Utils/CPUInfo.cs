using System;
using System.Management;

namespace MyWPF.Utils
{
    public class CPUInfo
    {

        #region 构造函数
        public CPUInfo()
        {
            GetCPUInfo();
        }

        #endregion

        #region 属性
        #region CPU名称

        public string CPUName { get; private set; }

        #endregion

        #region CPU序列号

        public string CPUID { get; private set; }

        #endregion

        #region CPU个数

        public int CPUCount { get; private set; }

        #endregion

        #region CPU制造商

        public string CPUManufacturer { get; private set; }

        #endregion

        #region 当前时钟频率

        public string CPUCurrentClockSpeed { get; private set; }

        #endregion

        #region 最大时钟频率

        public string CPUMaxClockSpeed { get; private set; }

        #endregion

        #region 外部频率

        public string CPUExtClock { get; private set; }

        #endregion

        #region 当前电压

        public string CPUCurrentVoltage { get; private set; }

        #endregion

        #region 二级缓存

        public string CPUL2CacheSize { get; private set; }

        #endregion

        #region 数据带宽

        public string CPUDataWidth { get; private set; }

        #endregion

        #region 地址带宽

        public string CPUAddressWidth { get; private set; }

        #endregion

        #region 使用百分比

        public float CPUUsedPercent { get; private set; }

        #endregion

        #region CPU温度

        public double CPUTemperature { get; private set; }

        #endregion
        #endregion

        #region GetCPUInfo
        private void GetCPUInfo()
        {
            #region 使用百分比
            GetCPULoadPercentage();
            #endregion

            CPUCount = Environment.ProcessorCount;  //CPU个数

            GetCPUCurrentTemperature();

            //实例化一个ManagementClass类，并将Win32_Processor作为参数传递进去，
            //这样就可以查询Win32_Processor这个类里面的一些信息了。
            var mClass = new ManagementClass("Win32_Processor");

            //获取Win32_Processor这个类的所有实例
            var moCollection = mClass.GetInstances();

            //对Win32_Processor这个类进行遍历
            foreach (ManagementObject mObject in moCollection)
            {
                CPUName = mObject["Name"].ToString();  //获取CPU名称
                CPUID = mObject["ProcessorId"].ToString();  //获取 CPU ID
                CPUManufacturer = mObject["Manufacturer"].ToString();  //获取CPU制造商
                CPUCurrentClockSpeed = mObject["CurrentClockSpeed"].ToString();  //获取当前时钟频率
                CPUMaxClockSpeed = mObject["MaxClockSpeed"].ToString();  //获取最大时钟频率
                CPUExtClock = mObject["ExtClock"].ToString();  //获取外部频率
                CPUCurrentVoltage = mObject["CurrentVoltage"].ToString();  //获取当前电压
                CPUL2CacheSize = mObject["L2CacheSize"].ToString();  //获取二级缓存
                CPUDataWidth = mObject["DataWidth"].ToString();  //获取数据带宽
                CPUAddressWidth = mObject["AddressWidth"].ToString();  //获取地址带宽
            }

        }

        public void GetCPUCurrentTemperature()
        {
            var mos = new ManagementObjectSearcher(@"root\wmi", @"select * from MSAcpi_ThermalZoneTemperature");
            foreach (ManagementObject mo in mos.Get())
            {
                CPUTemperature = Convert.ToDouble(Convert.ToDouble(mo["CurrentTemperature"].ToString()) - 2732) / 10;
            }
        }

        public void GetCPULoadPercentage()
        {
            var searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                CPUUsedPercent = float.Parse(queryObj["LoadPercentage"].ToString());
            }
        }
        #endregion
    }
}
