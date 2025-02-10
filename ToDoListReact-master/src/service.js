import axios from 'axios';

axios.defaults.baseURL = 'http://localhost:5172/tasks';

axios.interceptors.response.use(
  response => { return response; },
  error => {
    console.error('Response error:', error.response ? error.response.data : error.message);
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    try {
      const result = await axios.get(``);
      console.log(result.data);
      return result.data;
    } catch (e) {
      console.error('There was an error!', e); throw e;
    }
  },

  addTask: async (name) => {
    try {
      console.log('addTask', name)
      const result = await axios.post(``, { id: 0, name: name, isComplete: false })
      console.log(result.data);
      return result.data;
    } catch (e) {
      console.error('There was an error!', e); throw e;
    }
  },

  setCompleted: async (id, isComplete) => {
    try {
      console.log('setCompleted', { id, isComplete })
      await axios.put(`/${id}`, { id: id, name: "", isComplete: isComplete })
    } catch (e) {
      console.error('There was an error!', e); throw e;
    }
  },

  deleteTask: async (id) => {
    try {
      console.log('deleteTask')
      await axios.delete(`/${id}`, id);
    } catch (e) {
      console.error('There was an error!', e); throw e;
    }
  }
};