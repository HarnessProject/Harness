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

using System.Portable.Events;
 
using System.Portable.Runtime;
using System.Threading.Tasks;

#endregion

namespace System.Composition {
    public interface INotifyPropertyChange : INotify<IPropertyChangeEvent> {
        void PropertyChanged(string property, object oldvalue, object newValue);
        void OnPropertyChange(string property, DelegateAction<IPropertyChangeEvent> action);
    }

    public abstract class Notifier<T> : INotify<T> where T : IEvent {
        protected Notifier() {
            EventPipeline = new DelegatePipeline();
        }

        protected DelegatePipeline EventPipeline { get; set; }

        #region INotify<T> Members

        public async void Notify(T eEvent) {
            await EventPipeline.Process(eEvent);
        }

        public Task NotifyAsync(T eEvent) {
            return EventPipeline.Process(eEvent);
        }

        public void OnNotice(DelegateAction<T> action, DelegateFilter<T> filter) {
            EventPipeline.AddDelegate(action, filter);
        }

        #endregion
    }

    public abstract class NotifyPropertyChanged : Notifier<IPropertyChangeEvent>, INotifyPropertyChange {
        #region INotifyPropertyChange Members

        public void PropertyChanged(string property, object oldvalue, object newValue) {
            Notify(new PropertyChangeEvent {PropertyName = property, OldValue = oldvalue, NewValue = newValue});
        }

        public void OnPropertyChange(string property, DelegateAction<IPropertyChangeEvent> action) {
            OnNotice(action, x => x.PropertyName == property);
        }

        #endregion
    }
}