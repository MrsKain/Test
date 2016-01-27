/////////////////////////////////我的封装  rich1.0
/*01.获取style*//////////////////////////////////////////////////////
function richStyle(obj,sty){
	return obj.currentStyle? obj.currentStyle[sty] : getComputedStyle(obj)[sty];
};
/////////////////////////////////////////////////////////
/*02.简单模拟jquery选择器  暂时抄袭*/////////////////////////////////////////
function rich(q, o){
    //debugger;
    //查询表达式必须为字符串，并且不能为空。
    if(typeof(q)!=='string' || q == '') return [];
    
    //使用空格分割，只处理第一个表达式
    var ss = q.split(' ');
    
    //获取属性
    var attr = '';
    var s = ss[0].split(':')[0];
    if(s != ss[0])
        attr = ss[0].split(':')[1];
    
    var val = s.split('[')[0];
    if(val != s)
        val = s.split('[')[1].replace(/[",\]]/g,'');
    else
        val = '';
    s = s.split('[')[0];
    
    var obj = [];
    var sObj = null;
    //当父对象不存在时，使用document;
    o = o || document;
    switch(s.charAt(0))
    {
        case '#':
            //ID选择器
            sObj = document.getElementById(s.substr(1));
            if(sObj)obj.push(sObj);
            break;
        case '.':
            //类选择器
            var l = o.getElementsByTagName('*');
            var c = s.substr(1);
            for(var i=0; i<l.length; i++)
            if(l[i].className.search('\\b' + c + '\\b')!=-1)obj.push(l[i]);
            break;
        default:
            //根据tag获取元素
            obj = o.getElementsByTagName(s);
            break;
    }
    
    if(val)
    {
        //[t=val]筛选属性匹配
        var l = [];
        var a = val.split('=');
        for(var i=0; i<obj.length; i++)
            if(a.length == 2 && obj[i][a[0]] == a[1]) l.push(obj[i]);
        obj = l;
    }
    
    if(attr)
    {
        //: 筛选属性匹配
        var l = [];
        for(var i=0; i<obj.length; i++)
            if(obj[i][attr]) l.push(obj[i]);
        obj = l;
    }    
    
    if(ss.length > 1)
    {
        //递归处理表达式后续内容
        //父元素为已获取的所有元素
        var l = [];
        for(var i=0; i<obj.length; i++){
            var ll = arguments.callee(q.substr(ss[0].length+1), obj[i]);
            if(ll.tagName) l.push(ll);
            else
            for(var j=0; j<ll.length; j++)l.push(ll[j]);
        }
        obj = l;
    }
    
    if(sObj && ss.length == 1){
        //当为ID选择器时，直接返回对象。
        obj=sObj;
        if(obj)obj.length = 1;
    } else {
        //去除数组中重复元素
        var l = [];
        for(var i=0; i<obj.length; i++)obj[i].$isAdd = false;
        for(var i=0; i<obj.length; i++){
            if(!obj[i].$isAdd){
                obj[i].$isAdd = true;
                l.push(obj[i]);
            }
        }
        obj = l;
    }
    
    return obj;
}
///////////////////////////////////////////////////////////
/*03.简单运动的封装*/
function richMove(obj,json,rich_time,fn){
	clearInterval(obj.timer);
	var ste = 0;
	obj.timer = setInterval(function(){														
		var btn = true;
		for(var sty in json){						
			var loc = json[sty];
			//////////获取当前位置
			if(sty == 'opacity'){
			//////如果没有opacity属性 ie不能兼容///如果opacity是1  需要在css中写上1
				step = Math.round(richStyle(obj,'opacity')*100);
			}else{
				step = parseInt(richStyle(obj,sty));
			};	
			////计算速度  缓冲速度
			rich_time = rich_time? rich_time : 10;//默认10
			ste = (loc-step)/rich_time;
			ste = ste> 0 ? Math.ceil(ste) : Math.floor(ste);
			///////判断是否全部运动完成
			if(step!=loc){
				btn = false;
				//////赋值
				if(sty == 'opacity'){
					obj.style.opacity = (step + ste)/100;
					obj.style.filter = 'alpha(opacity='+step+ste+')';
				}else{
					obj.style[sty] = step + ste + 'px';
				};
			};
									
		};
		/////////////全部完成之后清除
		if(btn){
			clearInterval(obj.timer);
			//this的持续使用  回调函数
			fn&&fn.call(obj);
		};
						
	},30);	
};
//////////////////////////////////////////////////////////////////////
/*04.richPos获取绝对位置的封装*/
function richPos(obj){
	var pos ={left:0,top:0};
	while(obj){
		pos.left +=obj.offsetLeft;
		pos.top +=obj.offsetTop;
		obj = obj.offsetParent;
	};
	return pos;
};
//////////////////////////////////////////////////////////////////
/*04.暂时的计算 中间位置封装*/
function richCenter(obj){
	var wi0 = document.documentElement.clientWidth;
	var he0 = document.documentElement.clientHeight;
	var oBox_wi = obj.offsetWidth;
	var oBox_he = obj.offsetHeight;
	var cen = {left:(wi0-oBox_wi)/2,top:(he0-oBox_he)/2}
	return cen;	
};
//////////////////////////////////////////////////////////////////






































