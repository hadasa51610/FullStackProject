import axios from 'axios';

const url=process.env.REACT_APP_API_URL;
 console.log(url);
axios.defaults.baseURL=url;

axios.interceptors.response.use(
  response => { return response; },
  error => {
    console.error('Response error:', error.response ? error.response.data : error.message);
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
      const result = await axios.get(`/tasks`);
      console.log(result.data);
      return result.data;
  },

  addTask: async (name) => {
      console.log('addTask', name)
      const result = await axios.post(`/tasks`, { id: 0, name: name, isComplete: false })
      console.log(result.data);
      return result.data;
  },

  setCompleted: async (id, isComplete) => {
      console.log('setCompleted', { id, isComplete })
      await axios.put(`/tasks/${id}`, { id: id, name: "", isComplete: isComplete })
  },

  deleteTask: async (id) => {
      console.log('deleteTask')
      await axios.delete(`/tasks/${id}`, id);
  }
};