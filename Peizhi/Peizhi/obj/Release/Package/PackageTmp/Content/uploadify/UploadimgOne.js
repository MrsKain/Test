function UploadK()
{
    
    var file_count = 1;
    var num = 1;
    $("#upload_file").uploadify({
        'swf': '/Content/uploadify/uploadify.swf',//指定swf文件
        'uploader': '/Content/uploadify/uploadifyUpload.ashx',//调取后台处理方法
        'folder': '/Content/UploadFiles/images',//图片保存路径
        'fileTypeExts': '*.jpg; *.png; *.jpeg; *.gif; *.bmp',//文件上传类型后缀,不符合时有提醒
        'fileSizeLimit': "0",//上传文件大小限制数
        'auto': true,//选择后自动上传,默认值为true                
        'multi': false,//设置上传时是否可以选择多个,true可以,false禁止选中多个
        'method': 'post',//提交方式(get或者post),默认是post
        //'buttonText': '选择文件',
        'sizeLimit': '102400000',//改大
        'width': '60px',
        'height': '20px',
        'removeCompleted': true,
        'removeTimeout': 1,
        'uploadLimit': 999,//允许连续上传的次数,超过会提示
        'onUploadSuccess': function (file, data, respone) {
          
            var arr = data.split('|');
            var chgDisplay = $('.pic_demo');//div类名
            picDispaly({
                div: chgDisplay,
                url: arr[1]
            });
            function picDispaly(obj) {
                var img = new Image();
                img.src = "/Content/UploadFiles/images/" + obj.url;
                $(img).attr("data-url", obj.url);

                var imgList = $('<div class="imgBox"><span class="editImg"><i class="icon icon-edit"></i></span><span class="delImg">×</span><p class="imgInfo"><input type="text" name="imgIndex" class="imgIndex" value="' + num + '" /></p></div>');
                num += 1;
                file_count += 1;
                imgList.append(img);
                $(obj.div).append(imgList);

            }
            chgDisplay.find('.imgBox').mouseenter(function (e) {
                $(this).find('.delImg,.editImg').show();
            }).mouseleave(function (e) {
                $(this).find('.delImg,.editImg,.imgInfo').hide();
            });
            chgDisplay.find('.editImg').click(function (e) {
                $(this).parent().find('.imgInfo').show();
            });
            chgDisplay.find('.delImg').click(function (e) {
                $(this).parent().remove();
                file_count -= 1;
                if (file_count <= 1) {
                    $('#upload_file').show();
                }
            });
            if (file_count > 1) {
                $('#upload_file').hide();
            }
        },
       
        'onCancel': function (event, queueId, fileObj, data) {

        },
        'onUploadError': function (file, errorCode, errorMsg, errorString) {

        },
        'onError': function (event,ID,fileObj,errorObj) {
        alert(errorObj.type + ' Error: ' + errorObj.info);
    }
    });
}