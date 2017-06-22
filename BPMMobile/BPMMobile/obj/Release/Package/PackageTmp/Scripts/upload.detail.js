function upload() {


    //var picUrl = '/upload/';

    var picUrl = 'http://172.16.3.53:8040/BPM/YZSoft/Attachment/default.ashx';

    var me = this;

    $("#file").upload({

        uploadUrl: '/api/bpm/UploadFiles',
        
        previewWrap: '.upload-img-list',
        isFile: false,
        //previewUrl: picUrl + '{id}?w=120&h=120',		//图片预览路径
        //previewUrl: picUrl + '?{id}?w=120&h=120',
        previewUrl: picUrl + '?{id}',
        //上传开始
        onUploadStart: function (xhr) {
            var $progress = this.find('.progress', true);
            //保存进度节点
            $progress.html('0%').addClass('uploading');
            //上传图片限制为10张
            if ($('.pic-preview').length >= 10) {
                $('.upload-addbtn:not(.fileIdString)').hide();

            } else {

                var count = $('.upload-img-list >div').size();

                if (count <= 4) {
                    $('#uploaddiv').css('height', '7rem');


                } else if (count <= 8) {
                    $('#uploaddiv').css('height', '14rem');

                } else if (count <= 12) {
                    $('#uploaddiv').css('height', '21rem');
                }

            }

        },

        //上传成功
        onUploadSuccess: function (fileInfo) {
           

            this.getRoot$().data('fileid', fileInfo.id)
								   .addClass('success');
            //隐藏进度
            this.find('.progress', true).html('').removeClass('uploading');
            //显示删除按钮
            this.find('.del', true).fadeIn();

            //计算上传成功图片总大小
            // totalSize += fileInfo.size;
            //图片总大小未换算
            me.data.totalSize += fileInfo.size;

            var totalSize = me.data.totalSize;

            if (totalSize >= 1024 * 1024) {
                $('.totalsize i').text((totalSize / (1024 * 1024)).toFixed(2) + 'MB');
            } else {
                $('.totalsize i').text((totalSize / 1024).toFixed(2) + 'kb');
            }

            //上传图片信息
            me.data.photoInfo.push(fileInfo);
            var photsInfos = me.data.photoInfo;

            var photosInfosLength = photsInfos.length;

            $("#imageCount").html("已添加" + photosInfosLength + "张");


           

        },
        //上传进度
        onUploadProgress: function (loaded, total) {
            var $progress = this.find('.progress', true);
            //修改进度
            $progress.html(Math.floor(loaded / total * 100) + '%');
            //$('.ceshi').html(loaded);
        },
        //上传失败
        onUploadFail: function (xhr) {
            var $progress = this.find('.progress', true);
            $progress.removeClass('uploading');
        },
        //删除图片 
        onDelImage: function (fileInfo) {
            //上传图片小于11张
            if ($('.pic-preview').length <= 10) {
                $('.upload-addbtn:not(.fileIdString)').show();
            }

          

        }

    });




}












