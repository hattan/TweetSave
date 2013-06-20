var app = angular.module('TwitterClient');

app.directive('searchForm', function () {

    return {
        restrict: 'E',
        scope: {},
        templateUrl: '../partials/reverse_template.html',
        controller: function ($scope, $http, $rootScope) {

            $scope.doSearch = function (term) {
                var url = '/api/twitter/?q=' + term;

                $http.get(url)
                    .success(function (data) {
                        $rootScope.results = data.statuses;
                    });
            };
        },
        replace: true, 
        link: function (scope, elem, attr) {
            
        }
    };
});

app.directive('slider', function () {
    return {
        restrict: 'E',
        scope: { value: '=value' },
        templateUrl: '../partials/slider.html',
        controller: function ($scope, $element, $attrs, $rootScope) {
                $element.slider().on('slide', function (ev) {
                    $scope.$apply(function () {
                        $rootScope[$attrs.value] = ev.value;
                    });
                });
        },
        replace: true,
        link: function (scope, elem, attr) {}
    };
});

app.directive('top', function () {
    return {
        transclude: 'element',
        compile: function(element, attr, linker) {
            return function($scope, $element, $attr) {
                var myLoop = $attr.top,
                    take = $attr.take,
                    limit = 1;
                    match = myLoop.match(/^\s*(.+)\s+in\s+(.*?)\s*(\s+track\s+by\s+(.+)\s*)?$/),
                    indexString = match[1],
                    collectionString = match[2],
                    parent = $element.parent(),
                    elements = [],
                    collectionCache=null;

                $scope.$watch(take, function(val) {
                    if (collectionCache != null) {
                        limit = val;
                        if (limit == undefined || limit == "") {
                            limit = 1;
                        }
                        
                        process(collectionCache);
                    }
                });
                
                // $watchCollection is called everytime the collection is modified
                $scope.$watchCollection(collectionString, function(collection) {
                    process(collection);
                });
                
                function process(collection) {
                    if (collection === undefined) return;
                    collectionCache = collection;
                    var i, block, childScope;

                    // check if elements have already been rendered
                    if (elements.length > 0) {
                        // if so remove them from DOM, and destroy their scope
                        for (i = 0; i < elements.length; i++) {
                            elements[i].el.remove();
                            elements[i].scope.$destroy();
                        }
                        ;
                        elements = [];
                    }


                  
                   
                    var len = limit < collection.length ? limit : collection.length;
                    

                    for (i = 0; i < len; i++) {
                        // create a new scope for every element in the collection.
                        childScope = $scope.$new();

                        // Set $scope.item = item in collection
                        childScope[indexString] = collection[i];

                        linker(childScope, function (clone) {
                            // clone the transcluded element, passing in the new scope.
                            parent.append(clone); // add to DOM
                            block = {};
                            block.el = clone;
                            block.scope = childScope;
                            elements.push(block);
                        });
                    }
                }
            };
        }
    };
});
