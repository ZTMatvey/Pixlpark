import { useState, useEffect } from 'react';
import './App.css';
import CustomButton from './CustomButton';
import LabelInput from './LabelInput';
import { render } from '@testing-library/react';
import { Routes, BrowserRouter, Route } from 'react-router-dom';
import Register from './Register'
import Success from './Success';
import Fail from './Fail';

function App() {
  return (
    <BrowserRouter>
      <div className='wrapper'>
        <div className='register--container'>
          <Routes>
            <Route path='/register' Component={Register}/>
            <Route path='/register/success' Component={Success}/>
            <Route path='/register/fail' Component={Fail}/>
          </Routes>
        </div>
      </div>
    </BrowserRouter>);
}

export default App;
