// new Vue({
//     el: '#headerinit',
    
//     data: {
//         mode: 'standart',
//         personalInfo: [0],
//         lengthArray: 0,
//         modalTitle: '',
//         dateArray: [],
//         holidayDays: 0,
//         hospitalDays: 0,
//         assignmentDays: 0,
//         taketimeoffDays: 0,
//         modalEmployeeActive: false,
//         modalOperation: null,
//         modalType: 0,
//         firstInitial: "",
        
//     },
//     mounted() {
//         this.customerId = document.getElementById('idCu').textContent;
//         this.companyId = document.getElementById('idCo').textContent;
//         this.employeeId = document.getElementById('idEm').textContent;
//         this.rol = document.getElementById('rol').textContent;
//         this.Init();

//     },
//     methods: {
//         async Init() {
//             this.personalInfo = await this.getEmployee();
//             this.firstInitial = this.personalInfo.firstName[0];
//             console.log(this.firstInitial);
//         },
      
//         async getEmployee() {
//             let response = await axios.get("/Employees/GetEmployee", {
//                 params: {
//                     idCu: this.selectedCustomerId,
//                     idCo: this.selectedCompanyId,
//                     idEm: this.employeeId
//                 }
//             });
//             return response.data;
//         },

//     }
// });