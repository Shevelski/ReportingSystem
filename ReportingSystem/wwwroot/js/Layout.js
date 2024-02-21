$(document).ready(function () {
    // Function to load content dynamically
    function loadContent(url, target) {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (data) {

                $(target).empty();

                // Load new content
                $(target).html(data);
            },
            error: function () {
                alert('Error loading content');
            }
        });
    }

    // Handle navigation clicks
    $('.nav-link').on('click', function (e) {
        e.preventDefault();
        var url = $(this).attr('href');
        var target = $(this).data('target');
        loadContent(url, target);
    });


    $('.navbar-brand').on('click', function (e) {
        e.preventDefault();
        var url = $(this).attr('href');
        var target = $(this).data('target');
        loadContent(url, target);
    });

    $('.linkfooter').on('click', function (e) {
        e.preventDefault();
        var url = $(this).attr('href');
        var target = $(this).data('target');
        loadContent(url, target);
        if ($('.navbar-brand').hasClass('active')) {
            $('.navbar-brand').removeClass('active');
        }
        if ($('.nav-link').hasClass('active')) {
            $('.nav-link').removeClass('active');
        }
    });


    //Set active element in header
    $(document).ready(function () {
        $('.nav-link').click(function () {
            if ($('.navbar-brand').hasClass('active')) {
                $('.navbar-brand').removeClass('active');
            }
            $('.nav-link').removeClass('active');
            $(this).closest('.nav-link').addClass('active');
        });
    });

    $(document).ready(function () {
        $('.navbar-brand').click(function () {
            if ($('.nav-link').hasClass('active')) {
                $('.nav-link').removeClass('active');
            }
            $(this).closest('.navbar-brand').addClass('active');
        });
    });
});