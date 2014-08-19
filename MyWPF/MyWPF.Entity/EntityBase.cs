using System;
using MyWPF.MVVMBase;

namespace MyWPF.Entity
{
    /// <summary>
    /// 实体基类
    /// </summary>
    [Serializable]
    public class EntityBase : NotifyObject
    {
        private string _id;

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        /// <value>The Id</value>
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(() => Id);
            }
        }
       
    }
}
