using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Composition;
using System.Composition.CaliburnMicro;
using System.Linq;
using System.Linq.Expressions;
using System.Net.PeerToPeer.Collaboration;
using System.Portable.Runtime.Reflection;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autofac.Core;

namespace Sandbox.ViewModels
{
    public class MainViewModel : Shell {
        private readonly Reactive<int> _number = new Reactive<int>(0);

        public MainViewModel() {
            _number.Subscribe(x => OnPropertyChanged(new PropertyChangedEventArgs("Number")));
            
        }
        
        public int Number {
            get {
                return _number.Value;
            }
            set {
                _number.Value = value;
            }
        }

        public void Increment() {
            Number += 1;
        }

        public void Decrement() {
            Number -= 1;
        }

       
    }
}
