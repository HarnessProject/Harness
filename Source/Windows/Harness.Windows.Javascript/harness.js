(function() {
    var HarnessApplication, ProxyObject;

    HarnessApplication = (function() {

        function HarnessApplication() {
        }

        HarnessApplication.prototype.proxy = new Harness.Windows.Proxy;

        HarnessApplication.prototype.toObject = function(s) {
            return JSON.parse(s);
        };

        HarnessApplication.prototype.toJson = function(o) {
            return JSON.stringify(o);
        };

        HarnessApplication.prototype.resolve = function(type) {
            return new ProxyObject(proxy.resolve(type), this);
        };

        return HarnessApplication;

    })();

    ProxyObject = (function() {

        function ProxyObject(o, X) {
            var f, m, obj, p, str, _i, _j, _k, _len, _len1, _len2, _ref, _ref1, _ref2;
            str = X.toJson;
            obj = X.toObject;
            _ref = o.methods;
            for (_i = 0, _len = _ref.length; _i < _len; _i++) {
                m = _ref[_i];
                this.prototype[m] = function() {
                    return obj(o.invokeMethod(m, str(arguments)));
                };
            }
            _ref1 = o.fields;
            for (_j = 0, _len1 = _ref1.length; _j < _len1; _j++) {
                f = _ref1[_j];
                this.prototype[f] = o.getField(f);
            }
            _ref2 = o.properties;
            for (_k = 0, _len2 = _ref2.length; _k < _len2; _k++) {
                p = _ref2[_k];
                Object.defineProperty(this, p, {
                    get: function() {
                        return obj(o.getProperty(p));
                    },
                    set: function(newVal) {
                        return o.setProperty(p, str(newVal));
                    },
                    configurable: false
                });
            }
        }

        return ProxyObject;

    })();

    window.X = new HarnessApplication;

}).call(this);