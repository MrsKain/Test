var UITree = function () {

    return {
        //main function to initiate the module
        init: function () {

            var DataSourceTree = function (options) {
                this._data = options.data;
                this._delay = options.delay;
            };

            DataSourceTree.prototype = {

                data: function (options, callback) {
                    var self = this;

                    setTimeout(function () {
                        var data = $.extend(true, [], self._data);

                        callback({ data: data });

                    }, this._delay)
                }
            };

            // INITIALIZING TREE
            var treeDataSource = new DataSourceTree({
                data: [
                   
                ],
                delay: 400
            });

            var treeDataSource2 = new DataSourceTree({
                data: [

                ],
                delay: 400
            });
            var treeDataSource3 = new DataSourceTree({
                data: [

                ],
                delay: 400
            });

            var treeDataSource4 = new DataSourceTree({
                data: [
                   
                   
                ],
                delay: 400
            });

            var treeDataSource5 = new DataSourceTree({
                data: [
                 
                ],
                delay: 400
            });

            var treeDataSource6 = new DataSourceTree({
                data: [
                   
                ],
                delay: 400
            });

            $('#MyTree').tree({
                dataSource: treeDataSource,
                multiSelect: true,
                loadingHTML: '<div class="tree-loading"><i class="fa fa-rotate-right fa-spin"></i></div>'
            });


            $('#MyTree2').tree({
                dataSource: treeDataSource2,
                multiSelect: true,
                loadingHTML: '<div class="tree-loading"><i class="fa fa-rotate-right fa-spin"></i></div>'
            });

            $('#MyTree3').tree({
                dataSource: treeDataSource3,
                multiSelect: true,
                loadingHTML: '<div class="tree-loading"><i class="fa fa-rotate-right fa-spin"></i></div>'
            });

            $('#MyTree4').tree({
                selectable: false,
                dataSource: treeDataSource4,
                loadingHTML: '<div class="tree-loading"><i class="fa fa-rotate-right fa-spin"></i></div>'
            });

            $('#MyTree5').tree({
                selectable: false,
                dataSource: treeDataSource5,
                loadingHTML: '<div class="tree-loading"><i class="fa fa-rotate-right fa-spin"></i></div>'
            });

            $('#MyTree6').tree({
                selectable: false,
                dataSource: treeDataSource6,
                loadingHTML: '<div class="tree-loading"><i class="fa fa-rotate-right fa-spin"></i></div>'
            });
        }

    };

}();