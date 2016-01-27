/* popModal - 23.05.14 */
/* popModal */



/* notifyModal */



/* hintModal */



/* dialogModal */
(function($) {
	$.fn.dialogModal = function(method) {
		var elem = $(this),
		elemObj,
		elemContObj,
		elemClass = 'dialogModal',
		prevBut = 'dialogPrev',
		nextBut = 'dialogNext',
		_options,
		animTime;
	
		var methods = {
			init : function(params) {
				var _defaults = {
					onOkBut: function() {return true;},
					onCancelBut: function() {},
					onLoad: function() {},
					onClose: function() {}
				};
				_options = $.extend(_defaults, params);

				$('html.' + elemClass + 'Open').off('.' + elemClass + 'Event').removeClass(elemClass + 'Open');
				$('.' + elemClass + ' .' + prevBut + ', .' + elemClass + ' .' + nextBut).off('click');
				$('.' + elemClass).remove();

				var currentDialog = 0,
				maxDialog = elem.length - 1;

				dialogMain = $('<div class="' + elemClass + '"></div>'),
				dialogContainer = $('<div class="' + elemClass + '_container"></div>'),
				dialogCloseBut = $('<button type="button" class="close">&times;</button>'),
				dialogBody = $('<div class="' + elemClass + '_body"></div>');
				dialogMain.append(dialogContainer);
				dialogContainer.append(dialogCloseBut, dialogBody);
				dialogBody.append(elem[currentDialog].innerHTML);
				
				if (maxDialog > 0) {
					dialogContainer.prepend($('<div class="' + prevBut + ' notactive"></div><div class="' + nextBut + '"></div>'));
				}
				$('body').append(dialogMain);
				elemObj = $('.' + elemClass);
				elemContObj = $('.' + elemClass + '_container');
				getAnimTime();

				if (_options.onLoad && $.isFunction(_options.onLoad)) {
					_options.onLoad();
				}

				elemObj.on('destroyed', function() {
					if (_options.onClose && $.isFunction(_options.onClose)) {
						_options.onClose();
					}
				});
				
				centerDialog();
				
				function centerDialog() {
					var dialogHeight = elemContObj.outerHeight(),
					windowHeight = $(window).height();
					if (windowHeight > dialogHeight + 80) {
						elemContObj.css({
							marginTop: ($(window).height() - dialogHeight) / 2 + 'px'
						});	
					} else {
						elemContObj.css({
							marginTop: '60px'
						});						
					}
					
					$('body').addClass(elemClass + 'Open');
					elemObj.addClass('open');

					setTimeout(function() {
						elemObj.addClass('open');
						elemContObj.css({
							marginTop: parseInt(elemContObj.css('marginTop')) - 20 + 'px'
						});	
					}, animTime);
					
					bindFooterButtons();
				}
				
				function bindFooterButtons() {
					elemObj.find('[data-dialogModalBut="close"]').bind('click', function() {
						dialogModalClose();
					});

					elemObj.find('[data-dialogModalBut="ok"]').bind('click', function(event) {
						var ok;
						if (_options.onOkBut && $.isFunction(_options.onOkBut)) {
							ok = _options.onOkBut(event);
						}
						if (ok !== false) {
							dialogModalClose();
						}
					});

					elemObj.find('[data-dialogModalBut="cancel"]').bind('click', function() {
						if (_options.onCancelBut && $.isFunction(_options.onCancelBut)) {
							_options.onCancelBut();
						}
						dialogModalClose();
					});
				}

				elemObj.find('.' + prevBut).bind('click', function() {
					if (currentDialog > 0) {
						--currentDialog;
						if (currentDialog < maxDialog) {
							elemObj.find('.' + nextBut).removeClass('notactive');
						}
						if (currentDialog == 0) {
							elemObj.find('.' + prevBut).addClass('notactive');
						}
						dialogBody.empty().append(elem[currentDialog].innerHTML);
						centerDialog();
					}
				});
				
				elemObj.find('.' + nextBut).bind('click', function() {
					if (currentDialog < maxDialog) {
						++currentDialog;
						if (currentDialog > 0) {
							elemObj.find('.' + prevBut).removeClass('notactive');
						}
						if (currentDialog == maxDialog) {
							elemObj.find('.' + nextBut).addClass('notactive');
						}
						dialogBody.empty().append(elem[currentDialog].innerHTML);
						centerDialog();
					}
				});

				elemObj.find('.close').bind('click', function() {
					dialogModalClose();
				});
				
				$('html').on('keydown.' + elemClass + 'Event', function(event) {
					if (event.keyCode == 27) {
						dialogModalClose();
					} else if (event.keyCode == 37) {
						elemObj.find('.' + prevBut).click();
					} else if (event.keyCode == 39) {
						elemObj.find('.' + nextBut).click();
					}
				});
					
			},
			hide : function() {
				dialogModalClose();
			}
		};
		
		function dialogModalClose() {
		var elemObj = $('.' + elemClass);
			setTimeout(function() {
				elemObj.removeClass('open');
				setTimeout(function() {
					elemObj.remove();
					$('body').removeClass(elemClass + 'Open');
					$('html.' + elemClass + 'Open').off('.' + elemClass + 'Event').removeClass(elemClass + 'Open');
					elemObj.find('.' + prevBut).off('click');
					elemObj.find('.' + nextBut).off('click');
				}, animTime);
			}, animTime);
		}
		
		function getAnimTime() {
			if (!animTime) {
				animTime = elemObj.css('transitionDuration');
				if (animTime != undefined) {
					animTime = animTime.replace('s', '') * 1000;
				} else {
					animTime = 0;
				}
			}
		}

		if (methods[method]) {
			return methods[method].apply( this, Array.prototype.slice.call(arguments, 1));
		} else if (typeof method === 'object' || ! method) {
			return methods.init.apply( this, arguments );
		}

	}
	
	$('* [data-dialogModalBind]').bind('click', function() {
		var elemBind = $(this).attr('data-dialogModalBind');
		$(elemBind).dialogModal();
	});

  $.event.special.destroyed = {
    remove: function(o) {
      if (o.handler) {
        o.handler();
      }
    }
  }
})(jQuery);


/* titleModal */
