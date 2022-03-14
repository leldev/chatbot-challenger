import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Login from './components/Login';
import Register from './components/Register';
import Logout from './components/Logout';
import Rooms from './components/Rooms';

import './custom.css'

function App() {
    return (
        <Layout>
            <Route exact path='/' component={Home} />
            <Route exact path='/login' component={Login} />
            <Route exact path='/register' component={Register} />
            <Route path='/logout' component={Logout} />
            <Route path='/rooms/:roomId?' component={Rooms} />
        </Layout >
    );
}

export default App;