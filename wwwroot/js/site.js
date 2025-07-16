$(function() {
    // Initialize all page functionality
    initMeetingMinutesPage();
    initMainFormPage();
});

function initMeetingMinutesPage() {
    // Only run if we're on the meeting minutes page
    if ($('#meetingMinutesTableBody').length) {
        loadMeetingMinutes();
        
        // Set up refresh button
        $('#refreshBtn').on('click', loadMeetingMinutes);
    }
}

function initMainFormPage() {
    // Only run if we're on the main form page
    if ($('#customerDropdown').length) {
        // Load customers by type
        function loadCustomers(type) {
            $.ajax({
                url: `/api/Customer/GetCustomers?type=${type}`,
                type: 'GET',
                success: function (data) {
                    const dropdown = $('#customerDropdown');
                    dropdown.empty().append('<option selected disabled>Select customer name</option>');
                    $.each(data, function (i, customer) {
                        dropdown.append(`<option value="${customer.id}">${customer.name}</option>`);
                    });
                },
                error: function () {
                    alert('Failed to load customer list.');
                }
            });
        }

        // Load products/services
        function loadProducts() {
            $.ajax({
                url: '/api/Product/GetAll',
                type: 'GET',
                success: function (data) {
                    const dropdown = $('#productDropdown');
                    dropdown.empty().append('<option selected disabled>Select product/service interested</option>');
                    $.each(data, function (i, item) {
                        dropdown.append(`<option value="${item.id}" data-unit="${item.unit}">${item.name}</option>`);
                    });
                },
                error: function () {
                    alert('Failed to load product list.');
                }
            });
        }

        // Load DisplayProduct records and render in table
        function loadDisplayProducts() {
            $.ajax({
                url: '/api/Product/DisplayProducts',
                type: 'GET',
                success: function (data) {
                    renderProductTable(data);
                },
                error: function () {
                    alert('Failed to load Display Products.');
                }
            });
        }

        // Render product table
        function renderProductTable(products) {
            const tbody = $('#productTable tbody');
            tbody.empty();

            if (products.length === 0) {
                tbody.append('<tr><td colspan="6" class="text-center">No matching records found</td></tr>');
                return;
            }

            $.each(products, function (index, item) {
                const row = `<tr data-id="${item.id}">
                                <td>${index + 1}</td>
                                <td>${item.name}</td>
                                <td>${item.quantity}</td>
                                <td>${item.unit}</td>
                                <td>
                                    <button type="button" class="btn btn-warning btn-sm btn-edit" data-bs-toggle="modal" data-bs-target="#editModal">
                                        Edit
                                    </button>
                                </td>
                                <td>
                                    <button type="button" class="btn btn-danger btn-sm btn-delete">
                                        Delete
                                    </button>
                                </td>
                            </tr>`;
                tbody.append(row);
            });
        }

        // Initial loads
        loadCustomers('Corporate');
        loadProducts();
        loadDisplayProducts();

        // Switch customers based on radio selection
        $('input[name="customerType"]').on('change', function () {
            const selectedType = $(this).attr('id') === 'corporate' ? 'Corporate' : 'Individual';
            loadCustomers(selectedType);
        });

        // Auto-fill unit when product is selected
        $('#productDropdown').on('change', function () {
            const unit = $(this).find('option:selected').data('unit');
            $('#unit').val(unit);
        });

        // Set up edit modal
        let currentEditId = null;
        $('#productTable').on('click', '.btn-edit', function() {
            const row = $(this).closest('tr');
            currentEditId = row.data('id');
            
            $('#editId').val(currentEditId);
            $('#editName').val(row.find('td').eq(1).text());
            $('#editUnit').val(row.find('td').eq(3).text());
            $('#editQuantity').val(row.find('td').eq(2).text());
        });

        // Save edited product
        $('#saveEditBtn').on('click', function() {
            const quantity = $('#editQuantity').val();
            
            if (!quantity || quantity <= 0) {
                alert('Please enter a valid quantity');
                return;
            }

            $.ajax({
                url: `/api/Product/Edit/${currentEditId}`,
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify({
                    id: currentEditId,
                    name: $('#editName').val(),
                    unit: $('#editUnit').val(),
                    quantity: quantity
                }),
                success: function() {
                    $('#editModal').modal('hide');
                    loadDisplayProducts();
                },
                error: function() {
                    alert('Update failed.');
                }
            });
        });

        // Delete product
        $('#productTable').on('click', '.btn-delete', function() {
            if (!confirm('Are you sure you want to delete this product?')) return;
            
            const productId = $(this).closest('tr').data('id');
            
            $.ajax({
                url: `/api/Product/Delete/${productId}`,
                type: 'DELETE',
                success: function() {
                    loadDisplayProducts();
                },
                error: function() {
                    alert('Delete failed.');
                }
            });
        });

        // Add product
        $('#addRowBtn').on('click', function() {
            const productOption = $('#productDropdown option:selected');
            const productId = $('#productDropdown').val();
            const quantity = $('#quantityInput').val();
            const unit = $('#unit').val();

            if (!productId || !quantity || quantity <= 0) {
                alert("Please select a product and enter a valid quantity.");
                return;
            }

            const productData = {
                Name: productOption.text(),
                Quantity: quantity,
                Unit: unit
            };

            $.ajax({
                url: '/api/Product/Add',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(productData),
                success: function() {
                    // Clear form
                    $('#productDropdown').val('');
                    $('#quantityInput').val('');
                    $('#unit').val('');
                    
                    // Reload table
                    loadDisplayProducts();
                },
                error: function() {
                    alert('Failed to add product.');
                }
            });
        });

        // Form submission
        $('form').on('submit', function(e) {
            e.preventDefault();
            
            // Collect all form data
            const formData = {
                // First part (Master table)
                CustomerType: $('input[name="customerType"]:checked').attr('id') === 'corporate' ? 'Corporate' : 'Individual',
                CustomerId: $('#customerDropdown').val(),
                MeetingDate: $('input[type="date"]').val(),
                MeetingTime: $('input[type="time"]').val(),
                MeetingPlace: $('input[placeholder="Meeting place"]').val(),
                ClientAttendees: $('textarea[placeholder="Client attendees"]').val(),
                HostAttendees: $('textarea[placeholder="Host attendees"]').val(),
                
                // Second part (Detail table)
                Agenda: $('textarea[placeholder="Meeting agenda"]').val(),
                Discussion: $('textarea[placeholder="Meeting discussion"]').val(),
                Decision: $('textarea[placeholder="Meeting decision"]').val()
            };

            // Send data to server
            $.ajax({
                url: '/api/Customer/Save',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(formData),
                success: function(response) {
                    if (response.success) {
                        alert('Meeting minutes saved successfully!');
                        // Optionally reset the form
                        $('form')[0].reset();
                        $('#productTable tbody').html('<tr><td colspan="6" class="text-center">No matching records found</td></tr>');
                    } else {
                        alert('Error: ' + response.message);
                    }
                },
                error: function(xhr) {
                    alert('Error saving meeting minutes: ' + 
                        (xhr.responseJSON?.message || xhr.statusText));
                }
            });
        });
    }
}



function loadMeetingMinutes() {
    // Show loading state
    $('#meetingMinutesTableBody').html(`
        <tr>
            <td colspan="10" class="text-center py-4">
                <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Loading...</span>
                </div>
                <p class="mt-2 mb-0">Loading meeting minutes...</p>
            </td>
        </tr>
    `);

    $.ajax({
        url: '/api/Customer/GetMeetingMinutes',
        type: 'GET',
        success: function(data) {
            const tableBody = $('#meetingMinutesTableBody');
            tableBody.empty();
            
            if (data.length === 0) {
                tableBody.append(`
                    <tr>
                        <td colspan="10" class="text-center py-4 text-muted">
                            No meeting minutes found
                        </td>
                    </tr>
                `);
                return;
            }
            
            $.each(data, function(index, item) {
                const row = `
                <tr class="table-row">
                    <td class="id-cell">${item.id}</td>
                    <td class="type-cell">${item.customerType || 'N/A'}</td>
                    <td class="name-cell">${item.customerName || 'N/A'}</td>
                    <td class="date-cell">${formatDateTime(item.meetingDateTime)}</td>
                    <td class="place-cell">${item.meetingPlace || 'N/A'}</td>
                    <td class="attendees-cell">${formatAttendees(item.clientAttendees)}</td>
                    <td class="attendees-cell">${formatAttendees(item.hostAttendees)}</td>
                    <td class="agenda-cell">${item.agenda || 'N/A'}</td>
                    <td class="discussion-cell">${formatDiscussion(item.discussion)}</td>
                    <td class="decision-cell">${formatDecision(item.decision)}</td>
                </tr>`;
                tableBody.append(row);
            });
        },
        error: function(xhr) {
            console.error('Error loading meeting minutes:', xhr);
            $('#meetingMinutesTableBody').html(`
                <tr>
                    <td colspan="10" class="text-center py-4 text-danger">
                        <i class="fas fa-exclamation-circle mr-2"></i>
                        Failed to load meeting minutes. Please try again.
                    </td>
                </tr>
            `);
        }
    });
}

function formatDateTime(dateTimeString) {
    if (!dateTimeString) return 'N/A';
    try {
        const date = new Date(dateTimeString);
        return isNaN(date) ? 'N/A' : date.toLocaleString();
    } catch {
        return 'N/A';
    }
}

function formatAttendees(attendees) {
    if (!attendees) return 'N/A';
    const attendeeList = attendees.split(',').map(a => a.trim());
    return attendeeList.map(a => `<div class="attendee">${a}</div>`).join('');
}

function formatDiscussion(discussion) {
    if (!discussion) return 'N/A';
    const points = discussion.split('-').filter(p => p.trim().length > 0);
    return points.length > 0 
        ? `<ul class="discussion-points">${points.map(p => `<li>${p.trim()}</li>`).join('')}</ul>`
        : discussion;
}

function formatDecision(decision) {
    if (!decision) return 'N/A';
    const points = decision.split('-').filter(p => p.trim().length > 0);
    return points.length > 0 
        ? `<ul class="decision-points">${points.map(p => `<li>${p.trim()}</li>`).join('')}</ul>`
        : decision;
}

// Initialize on page load
$(document).ready(function() {
    loadMeetingMinutes();
    $('#refreshBtn').on('click', loadMeetingMinutes);
});