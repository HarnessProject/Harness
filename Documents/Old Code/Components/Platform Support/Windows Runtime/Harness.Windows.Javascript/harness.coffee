class HarnessApplication
    constructor: () ->
    
    proxy: new Harness.Windows.Proxy;
    toObject: (s) -> JSON.parse s
    toJson: (o) -> JSON.stringify o
    resolve: (type) -> new ProxyObject (proxy.resolve type),this 
    
class ProxyObject 
    constructor: (o, X) ->
        str = X.toJson
        obj = X.toObject

        for m in o.methods 
            this.prototype[m] = -> obj o.invokeMethod m, str arguments
        
        
        for f in o.fields
            this.prototype[f] = o.getField f
            
        
        for p in o.properties
            Object.defineProperty this, p, 
                get: -> obj o.getProperty p
                set: (newVal) -> o.setProperty p, str newVal
                configurable: false

window.X = new HarnessApplication;