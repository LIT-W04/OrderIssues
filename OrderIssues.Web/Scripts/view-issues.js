$(function() {
    populatePage();

    function populatePage() {
        var orderId = $(".issues-div").data('order-id');
        $.get('/home/GetIssuesForOrder', {orderId: orderId}, function(result) {
            var html = '';
            if (!result.order) {
                html = '<h2>No unresolved issues for this order</h2>';
            } else {
                html = `<h4>Unresolved issues for ${result.order.title} placed on ${formatMVCDate(result.order.date)}</h4><br />`;
                html += result.issues.map(issue => {
                    return `<div class='well'>
                            <p>${issue.note}</p>
                            <br />
                            <button class='btn btn-danger resolve' data-issue-id='${issue.id}'>Resolve</button>
                        </div>`;
                }).join('<br />');
            }
            $(".issues-div").html(html);
        });
    }

    $(".new-issue").on('click', function() {
        $("#note").val('');
        $(".modal").modal();
    });

    $(".save-note").on('click', function() {
        $.post('/home/addissue',
            { note: $("#note").val(), orderId: $(".issues-div").data('order-id') }, function () {
                populatePage();
                $(".modal").modal('hide');
            });
    });

    $(".issues-div").on('click', '.resolve', function() {
        var issueId = $(this).data('issue-id');
        $.post('/home/resolveissue', {issueId: issueId}, function() {
            populatePage();
        });
    });
});