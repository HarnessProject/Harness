#region ApacheLicense

// System.Portable.Base
// Copyright © 2013 Nick Daniels et all, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing 	terms.
// 
// You may not use this file except in compliance with its License(s).
// 
// You may obtain a copy of the Apache License, Version 2.0 at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 

#endregion

#region

using System.Linq.Expressions;
using System.Portable;
using System.Portable.Events;
 
using System.Portable.Runtime;
using System.Reflection;

#endregion

namespace System.Composition {
    public interface INotifyPropertyChange : INotify<IPropertyChangeEvent> {
        void PropertyChanged(string property, object oldvalue, object newValue);
        void OnPropertyChange(string property, Action<IPropertyChangeEvent> action);
    }

    public abstract class NotifyPropertyChanged : Notifier<IPropertyChangeEvent>, INotifyPropertyChange {
        #region INotifyPropertyChange Members
        public string Message { get; set; }
        public void ChangeProperty<TY>(Expression<Func<TY>> prop, TY newValue) {
            var member = (MemberExpression) prop.Body;
            var p = ((PropertyInfo) member.Member);
            var old = p.GetValue(this, null);
            p.SetValue(this, newValue, null);
            PropertyChanged(p.Name, old, newValue);
        }

        public void PropertyChanged(string property, object oldvalue, object newValue) {
            Notify(new PropertyChangeEvent {PropertyName = property, OldValue = oldvalue, NewValue = newValue});
        }

        public void OnPropertyChange(string property, Action<IPropertyChangeEvent> action) {
            OnNotice(action, x => x.PropertyName == property);
        }

        #endregion
    }
}