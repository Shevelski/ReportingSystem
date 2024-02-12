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
        id: '',
    },
    mounted() {
        //setTimeout(() => {
        //    this.delay = true;
        //    document.getElementById('mask').classList.add('hidden'); // Приховуємо маску
        //}, 500);
    },
    methods: {
        async CheckEmail() {
            if (this.email != "") {
                let response = await axios.get("/Home/CheckEmail", {
                    params: {
                        email: this.email
                    }
                });

                this.data = response.data;
                console.log(this.data);
                this.status.type = this.data.authorizeStatusModel.authorizeStatusType;
                this.status.name = this.data.authorizeStatusModel.authorizeStatusName;
                this.Mode(this.status.type);
            }
        },
        async CheckPassword() {
            if (this.password != "") {
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
        },
    }
})
