// Everything is wrapped in the ready function to keep the window domain clean.
// Given a little more time I would probably put some of this into JS modules.
$(function () {
    // Data
    var user,
        currentPage = 1,
        maxPages,
        pageSize;

    // DOM Elements
    var userForm = $('#userForm'),
        userField = $('#user'),
        pageSizeField = $('#pageSize'),
        formErrorField = $('#formError'),
        todoDisplay = $('#todoList'),
        tasksTable = $('#tasks'),
        pageCountDisplay = $('#pageDisplay');

    // Generalized request function to process responses
    var request = function (path, cb) {
        $.ajax({
            url: path
        })
            .done(function (data) {
                if (data.success == false) {
                    cb(data.message, null);
                } else {
                    cb(null, data);
                }
            })
            .fail(function (xhr, status) {
                cb('Server Error: ' + status, null);
            });
    };

    var fillTasksTable = function (data) {
        tasksTable.empty();

        for (var t = 0; t < data.length; t++) {
            tasksTable.append($('<tr/>')
                .append($('<td/>').append($('<input/>').attr('type', 'checkbox').attr('checked', data[t].completed)))
                .append($('<td/>').text(data[t].title)));
        }
    };

    var getTasks = function () {
        // Since our API allows us to get all a users tasks if no page is given,
        // we offer the ability to see all of someone's tasks by omitting a page size
        var path = '/user/' + user.id + '/tasks';

        if (pageSize.length > 0) {
            path += '?page=' + currentPage + '&pageSize=' + pageSize
        }

        request(path, function (err, taskData) {
            if (!err) {
                maxPages = taskData.pages;
                pageCountDisplay.text(currentPage + '/' + maxPages);

                fillTasksTable(taskData.tasks);
            }
        });
    };

    var initializeList = function () {
        $('#nameDisplay').text(user.name + ', AKA ' + user.username);

        getTasks();

        todoDisplay.show();
    };

   userForm.submit(function (evt) {
        formErrorField.text('');

        currentPage = 1;
        request('/user/' + userField.val(), function (err, userData) {
            if (err) {
                formErrorField.text(err);
            } else {
                user = userData;
                pageSize = pageSizeField.val();
                initializeList();
                userForm.hide();
            }
        });

        evt.preventDefault();
    });

   $('#prevPage').click(function () {
       if (currentPage > 1) {
           currentPage--;
           getTasks();
       }
   });

   $('#nextPage').click(function () {
       if (currentPage < maxPages) {
            currentPage++;
            getTasks();
       }
   });

   $('#resetForm').click(function () {
       todoDisplay.hide();
       userForm.show();
   });
});