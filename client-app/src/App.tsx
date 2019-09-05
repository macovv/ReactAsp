import React, {Component} from 'react';
import {Header, Icon, List} from 'semantic-ui-react';
import logo from './logo.svg';
import './App.css';
import axios from 'axios';

class App extends Component{
  state = {
    values: []
  }

  componentDidMount(){
    axios.get('http://localhost:5000/api/values')
      .then((response) => {
        this.setState({
          values: response.data
        })
      })
    // this.setState({
    //   values: [{id: 1, name: "val1 2"}, {id:2, name: "fvfvrfv"}]
    // })
  }
  render () {
    return (
      <div>
        <Header as='h2'>
          <Icon name='plug' />
          <Header.Content>Uptime Guarantee</Header.Content>
        </Header>
        <List>
          <List.Item>            
          {this.state.values.map((value: any) => (
              <li key={value.id}>{value.name}</li>
            ))}</List.Item>
        </List>
      </div>
    );
  }
}

export default App;
