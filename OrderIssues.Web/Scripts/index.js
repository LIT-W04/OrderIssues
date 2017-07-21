$(function () {
    fillTable();

    function fillTable() {
        $.get('/home/GetIncompleteOrders', function (orders) {
            $("table tr:gt(0)").remove();
            orders.forEach(order => {
                var row = `<tr>
                            <td>${formatMVCDate(order.date)}</td>
                            <td>${order.title}</td>
                            <td>$${order.amount}</td>
                            <td>${order.resolvedIssueCount}</td>
                            <td>${order.unresolvedIssueCount}</td>
                            <td>
                                <button class ='btn btn-danger complete'
                                    ${order.unresolvedIssueCount !== 0 ? 'disabled': ''} data-order-id="${order.id}">Mark as completed</button>
                            </td>
                            <td>
                                <a href="/home/viewissues?orderid=${order.id}"
                                class="btn btn-primary">View Issues</a>
                            </td>


                           </tr>`;
                $("table").append(row);
            });
        });
    }
   
    $(".table").on('click', '.complete', function() {
        var orderId = $(this).data('order-id');
        $.post('/home/completeorder', {orderId: orderId}, function() {
            fillTable();
        });
    });
});