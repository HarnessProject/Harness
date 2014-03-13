using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Composition;
using System.Composition.CaliburnMicro;
using System.Linq;
using System.Linq.Expressions;
using System.Net.PeerToPeer.Collaboration;
using System.Portable.Events;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autofac.Core;
using LinqToDB.Data;
using MahApps.Metro.Controls.Dialogs;

namespace Sandbox.ViewModels
{
    public class MainViewModel : Shell {
        private readonly Reactive<int> _number = new Reactive<int>(0);

        public MainViewModel() {
            _number.Subscribe(x => OnPropertyChanged(new PropertyChangedEventArgs("Number")));

            var f = Filter.If<int>(x => x > 6).And(x => x < 10).AsFunc();

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
