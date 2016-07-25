// Copyright IBM Corp. 2015. All Rights Reserved.
// Node module: loopback-sdk-xm
// This file is licensed under the MIT License.
// License text available at https://opensource.org/licenses/MIT

var fs = require('fs');
var ejs = require('ejs');


module.exports = function generateServices(app, ngModuleName, apiUrl) {
  ngModuleName = ngModuleName || 'lbServices';
  apiUrl = apiUrl || '/';

  var models = describeModels(app);
  var servicesTemplate = fs.readFileSync(
    require.resolve('./services.template.ejs'),
    { encoding: 'utf-8' }
  );
  console.error('%j ', app.locals.settings.host.toString());
  return ejs.render(servicesTemplate, {
    moduleName: ngModuleName,
    models: models,
    urlBase: apiUrl.replace(/\/+$/, ''),
    host: app.locals.settings.host +":"+ app.locals.settings.port
  });
};

function describeModels(app) {
  var result = {};
  for(var model in app.models){
    model.get;
  }
  app.handler('rest').adapter.getClasses().forEach(function(c) {
    var name = c.name;
    if (!c.ctor) {
      // Skip classes that don't have a shared ctor
      // as they are not LoopBack models
      console.error('Skipping %j as it is not a LoopBack model', name);
      return;
    }
    c.methods.forEach(function fixArgsOfPrototypeMethods(method) {
      var ctor = method.restClass.ctor;
      if (!ctor || method.sharedMethod.isStatic) return;
      method.accepts = ctor.accepts.concat(method.accepts);
    });
    c.pluralName = c.sharedClass.ctor.pluralModelName;
    c.params =  app.models[c.name].definition.properties;
    c.baseModel = app.models[c.name].definition.settings.base;

    if(c.baseModel != null && typeof(c.baseModel) === "function"){
      c.baseModel = "";
    }
    if(app.models[c.name].definition._ids != null) {
      c.isGenerated = app.models[c.name].definition._ids[0].property.generated;
    }else{
      c.isGenerated = false;
    }
    c.relations = app.models[c.name].definition.settings.relations;
    c.acls = app.models[c.name].definition.settings.acls;
    c.validations = app.models[c.name].definition.settings.validations;
    c.isUser = c.sharedClass.ctor.prototype instanceof app.loopback.User ||
      c.sharedClass.ctor.prototype === app.loopback.User.prototype;
    result[name] = c;
  });

  buildScopes(result);

  return result;
}

var SCOPE_METHOD_REGEX = /^prototype.__([^_]+)__(.+)$/;

function buildScopes(models) {
  for (var modelName in models) {
    buildScopesOfModel(models, modelName);
  }
}

function buildScopesOfModel(models, modelName) {
  var modelClass = models[modelName];

  modelClass.scopes = {};
  modelClass.methods.forEach(function(method) {
    buildScopeMethod(models, modelName, method);
  });

  return modelClass;
}

// reverse-engineer scope method
// defined by loopback-datasource-juggler/lib/scope.js
function buildScopeMethod(models, modelName, method) {
  var modelClass = models[modelName];
  var match = method.name.match(SCOPE_METHOD_REGEX);
  if (!match) return;

  var op = match[1];
  var scopeName = match[2];
  var modelPrototype = modelClass.sharedClass.ctor.prototype;
  var targetClass = modelPrototype[scopeName]._targetClass;

  if (modelClass.scopes[scopeName] === undefined) {
    if (!targetClass) {
      console.error(
        'Warning: scope %s.%s is missing _targetClass property.' +
        '\nThe Angular code for this scope won\'t be generated.' +
        '\nPlease upgrade to the latest version of' +
        '\nloopback-datasource-juggler to fix the problem.',
        modelName, scopeName);
      modelClass.scopes[scopeName] = null;
      return;
    }

    if (!findModelByName(models, targetClass)) {
      console.error(
        'Warning: scope %s.%s targets class %j, which is not exposed ' +
        '\nvia remoting. The Angular code for this scope won\'t be generated.',
        modelName, scopeName, targetClass);
      modelClass.scopes[scopeName] = null;
      return;
    }

    modelClass.scopes[scopeName] = {
      methods: {},
      targetClass: targetClass
    };
  } else if (modelClass.scopes[scopeName] === null) {
    // Skip the scope, the warning was already reported
    return;
  }

  var apiName = scopeName;
  if (op == 'get') {
    // no-op, create the scope accessor
  } else if (op == 'delete') {
    apiName += '.destroyAll';
  } else {
    apiName += '.' + op;
  }

  // Names of resources/models in Angular start with a capital letter
  var ngModelName = modelName[0].toUpperCase() + modelName.slice(1);
  method.internal = 'Use ' + ngModelName + '.' + apiName + '() instead.';

  // build a reverse record to be used in ngResource
  // Product.__find__categories -> Category.::find::product::categories
  var reverseName = '::' + op + '::' + modelName + '::' + scopeName;

  var reverseMethod = Object.create(method);
  reverseMethod.name = reverseName;
  reverseMethod.internal = 'Use ' + ngModelName + '.' + apiName + '() instead.';
  // override possibly inherited values
  reverseMethod.deprecated = false;

  var reverseModel = findModelByName(models, targetClass);
  reverseModel.methods.push(reverseMethod);

  var scopeMethod = Object.create(method);
  scopeMethod.name = reverseName;
  // override possibly inherited values
  scopeMethod.deprecated = false;
  scopeMethod.internal = false;
  modelClass.scopes[scopeName].methods[apiName] = scopeMethod;
}

function findModelByName(models, name) {
  for (var n in models) {
    if (n.toLowerCase() == name.toLowerCase())
      return models[n];
  }
}
