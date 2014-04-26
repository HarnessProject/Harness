using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using Caliburn.Micro.Harness;
using Harness.Framework;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;
using Harness.Framework.Reactive;

namespace Sandbox.ViewModels
{
    public class MainViewModel : Shell {
        private readonly Reactive<int> _number = new Reactive<int>(0);

        public MainViewModel(IScope scope) : base(scope) {
            _number.Subscribe(x => OnPropertyChanged(new PropertyChangedEventArgs("Number")));
            var f = 
                Filter
                .If<int>(x => x > 6)
                .And(x => x < 10)
                .AsFunc();

            _number.Where(f).Subscribe(x => MessageBox.Show("Sweet Spot!")); 
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
