(function($){
	$('.follow-btn').bind( 'mouseenter', unfollow );
	$('.follow-btn').bind( 'mouseleave', follow );

	function unfollow() {
		var $this = $(this);

		$this.removeClass('primary');
		$this.addClass('tertiary');
		$this.text('Unfollow');
	}

	function follow() {
		var $this = $(this);

		$this.removeClass('tertiary');
		$this.addClass('primary');
		$this.text('Following');
	}
})(jQuery);