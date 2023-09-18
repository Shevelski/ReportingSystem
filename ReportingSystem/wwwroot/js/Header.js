new Vue({
    el: '#header_dropdown',
    data() {
      return {
        windowHeight: window.innerHeight,
        windowWidth: window.innerWidth
      }
    },
  
    mounted() {
      this.$nextTick(() => {
        window.addEventListener('resize', this.onResize);
      })
    },
  
    beforeDestroy() { 
      window.removeEventListener('resize', this.onResize); 
    },
  
    methods: {  
      onResize() {
        this.windowHeight = window.innerHeight
        this.windowWidth = window.innerWidth
      }
    }
  });