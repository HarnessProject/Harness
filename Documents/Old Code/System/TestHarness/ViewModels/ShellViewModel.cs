using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using Caliburn.Micro;
using Caliburn.Micro.Harness;
using Harness.Framework;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;
using Harness.Framework.Reactive;

namespace TestHarness.ViewModels
{
    public class ShellViewModel : Shell {
        private readonly Reactive<int> _number = new Reactive<int>(0);
        public override string DisplayName { get; set; } = "Reactive Model";
        
        public ShellViewModel() {
            _number.Subscribe(x => OnPropertyChanged(new PropertyChangedEventArgs("Number")));
            var f =
                Filter
                .If<int>(x => x > 6)
                .And(x => x < 10);

            _number.Where(f.AsFunc()).Subscribe(x => MessageBox.Show("Sweet Spot!")); 
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
