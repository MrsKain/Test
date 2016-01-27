/* ! jQuery v1.0 2016-1-4 */
// writer shoko

$(document).ready(function(){
	headNav();
  indexTab();
});

// 首页导航
function headNav(){
  $("#headnav ul li").click(function () {
    $("#headnav ul li").addClass("active").siblings("li").removeClass("active");
  });
};

// Tab选项卡
function indexTab(){
  $('#infr_ul').on('click','a',function(){
    var $self = $(this);//当前a标签
    var $active = $self.closest('li');//当前点击li
    var index = $active.prevAll('li').length;//当前索引

  $active.addClass('active').siblings('li').removeClass('active');
    $('#infr_ol').find('>li')[index==-1?'show':'hide']().eq(index).show();
  });
}