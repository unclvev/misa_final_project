using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagementSystem.Application.Dtos.CustomerDto.CustomerDto
{
    public class CreateCustomerRequest
    {
        [Required(ErrorMessage = "Tên khách hàng bắt buộc.")]
        [MaxLength(128, ErrorMessage = "Tên khách hàng tối đa 128 ký tự.")]
        public string FullName { get; set; } = default!;

        public string? Email { get; set; }

        [MaxLength(50, ErrorMessage = "Mã số thuế tối đa 50 ký tự.")]
        public string? TaxCode { get; set; }

        [MaxLength(255, ErrorMessage = "Địa chỉ giao hàng tối đa 255 ký tự.")]
        public string? ShippingAddress { get; set; }

        [Required(ErrorMessage = "Điện thoại bắt buộc (nếu không có Email).")]
        [RegularExpression(@"^\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*$", ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string Phone { get; set; } = default!;

        public ulong? CustomerTypeId { get; set; }

    }
}
