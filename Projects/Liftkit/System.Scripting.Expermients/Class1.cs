using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Pfz.TypeBuilding;
using Pfz.TypeBuilding.Blocks;

namespace System.Scripting.Expermients
{
    public class ProtectedExpression {
        protected FluentMethodBuilder ResultMethod { get; set; }
        protected FluentTryBuilder<FluentBodyBuilder> CurrentTry { get; set; }
        protected FluentCatchBuilder<FluentBodyBuilder> CurrentCatch { get; set; } 
        private bool _hasAssignerCatch = false;
        
        public ProtectedExpression() {
            ResultMethod = new FluentVoidMethodBuilder();
        }

        public ProtectedExpression WithValue<T>(Expression<Func<T>> value) {
            ResultMethod.AddExternal(value);
        }

        public ProtectedExpression Try(Expression<Action> action) {
            if (CurrentTry.IsNull()) CurrentTry = ResultMethod.Body.Try();
            CurrentTry.Do(action);
        }
        
        public ProtectedExpression Catch<T>(Expression<T> expression, Expression<Action<T>> handler) where T : Exception,new() {
            if (CurrentTry.IsNull()) throw new InvalidOperationException("Can't Catch Before You TRY");
            T exception = new T();
               
        }

    }
}
