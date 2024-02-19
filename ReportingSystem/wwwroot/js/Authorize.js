new Vue({
    el: '#Authorize',
    data: {
        delay: false,
        email: '',
        login: '',
        rol: '',
        password: '',
        mode: 0,
        data: {},
        status: { 
            type: -1,
            name: '',
        },
        load1: false,
        load2: false,
        id: '',
        isValidEmail: true,
    },
    mounted() {
        this.Mode(0);
    },
    methods: {
        async CheckEmail() {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            this.isValidEmail = emailRegex.test(this.email);
            this.$refs.emailInput.classList.toggle('is-invalid', !this.isValidEmail);

            if (this.email != "" && this.isValidEmail) {
                this.load1 = true;
                let response = await axios.get("/Home/CheckEmail", {
                    params: {
                        email: this.email
                    }
                });
                this.data = response.data;
                this.status.type = this.data.authorizeStatusModel.authorizeStatusType;
                this.status.name = this.data.authorizeStatusModel.authorizeStatusName;
                this.Mode(this.status.type);
                this.load1 = false;
            }
            
        },
        modeChange(newMode) {
            if (this.mode == 0 || this.mode == 2) {
                const form = this.$refs.emailForm;
                const input = this.$refs.emailInput;
                if (form && input) {
                    this.$nextTick(() => {
                        input.focus();
                    });
                }
            }
            if (this.mode == 1 || this.mode == 4) {
                const form = this.$refs.passwordForm;
                const input = this.$refs.passwordInput;
                if (form && input) {
                    input.focus();
                }
            }
        },
        async CheckPassword() {
            
            if (this.password != "") {
                this.load2 = true;
                let response = await axios.get("/Home/CheckPassword", {
                    params: {
                        email: this.email,
                        password: this.password
                    }
                });

                this.data = response.data;
                this.status.type = this.data.authorizeStatusModel.authorizeStatusType;
                this.status.name = this.data.authorizeStatusModel.authorizeStatusName;
                this.Mode(this.status.type);
                this.load2 = false;
            }
            
        },
        Mode(type) {

            if (type == 0) {
                this.mode = 0;
            }
            if (type == 1) {
                this.mode = 1;
            }
            if (type == 2) {
                this.mode = 2;
            }
            if (type == 3) {
                this.mode = 3;
            }
            if (type == 4) {
                this.mode = 4;
            }
            this.modeChange(this.mode);
        },
    }
})
