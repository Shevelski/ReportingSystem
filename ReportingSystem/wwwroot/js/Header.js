new Vue({
    el: '#header_dropdown',
    
    data() {
      return {
        windowHeight: window.innerHeight,
        windowWidth: window.innerWidth,
        mode: 'standart',
        personalInfo: [0],
        lengthArray: 0,
        modalTitle: '',
        dateArray: [],
        holidayDays: 0,
        hospitalDays: 0,
        assignmentDays: 0,
        taketimeoffDays: 0,
        modalEmployeeActive: false,
        modalOperation: null,
        modalType: 0,
        firstInitial: "",
        secondInitial: ""
      }
    },
    mounted() {
      this.$nextTick(() => {
        window.addEventListener('resize', this.onResize);
      });
      this.customerId = document.getElementById('idCu').textContent;
      this.companyId = document.getElementById('idCo').textContent;
      this.employeeId = document.getElementById('idEm').textContent;
      this.rol = document.getElementById('rol').textContent;
      this.Init();
    },
    beforeDestroy() { 
      window.removeEventListener('resize', this.onResize); 
    },
    methods: {  
        async Init() {
        this.personalInfo = await this.getEmployee();
        this.firstInitial = this.personalInfo.firstName[0];
        this.secondInitial = this.personalInfo.secondName[0];
    },
  
    async getEmployee() {
        let response = await axios.get("/Employees/GetEmployee", {
            params: {
                idCu: this.customerId,
                idCo: this.companyId,
                idEm: this.employeeId
            }
        });
        return response.data;
    },
    
      onResize() {
        this.windowHeight = window.innerHeight
        this.windowWidth = window.innerWidth
      }
    }
  });