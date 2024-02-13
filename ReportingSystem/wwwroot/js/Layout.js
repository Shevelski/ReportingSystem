//$(document).ready(function () {
//    function loadContent(url, target) {
//        $.ajax({
//            url: url,
//            type: 'GET',
//            success: function (data) {

//                $(target).empty();
//                $(target).html(data);
//            },
//            error: function () {
//                alert('Error loading content');
//            }
//        });
//    }

//    $('.nav-link').on('click', function (e) {
//        e.preventDefault();
//    var url = $(this).attr('href');
//    var target = $(this).data('target');
//    loadContent(url, target);
//    });


//    $('.navbar-brand').on('click', function (e) {
//        e.preventDefault();
//    var url = $(this).attr('href');
//    var target = $(this).data('target');
//    loadContent(url, target);
//        });

//    $('.linkfooter').on('click', function (e) {
//        e.preventDefault();
//    var url = $(this).attr('href');
//    var target = $(this).data('target');
//    loadContent(url, target);
//    if ($('.navbar-brand').hasClass('active')) {
//        $('.navbar-brand').removeClass('active');
//            }
//    if ($('.nav-link').hasClass('active')) {
//        $('.nav-link').removeClass('active');
//            }
//        });

//    $(document).ready(function () {
//        $('.nav-link').click(function () {
//            if ($('.navbar-brand').hasClass('active')) {
//                $('.navbar-brand').removeClass('active');
//            }
//            $('.nav-link').removeClass('active');
//            $(this).closest('.nav-link').addClass('active');
//        });
//        });

//    $(document).ready(function () {
//        $('.navbar-brand').click(function () {
//            if ($('.nav-link').hasClass('active')) {
//                $('.nav-link').removeClass('active');
//            }
//            $(this).closest('.navbar-brand').addClass('active');
//        });
//    });
//});

<script>
    $(document).ready(function () {
        function loadContent(url, target) {
            $.ajax({
                url: url,
                type: 'GET',
                success: function (data) {
                    $(target).empty();
                    $(target).html(data);
                },
                error: function () {
                    alert('Error loading content');
                }
            });
        }

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

    $('.nav-link').click(function () {
            if ($('.navbar-brand').hasClass('active')) {
        $('.navbar-brand').removeClass('active');
            }
    $('.nav-link').removeClass('active');
    $(this).closest('.nav-link').addClass('active');
        });

    $('.navbar-brand').click(function () {
            if ($('.nav-link').hasClass('active')) {
        $('.nav-link').removeClass('active');
            }
    $(this).closest('.navbar-brand').addClass('active');
        });
    });
</script>