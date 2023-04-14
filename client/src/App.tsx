import { useState } from 'react';
import './App.css';
import CustomButton from './CustomButton';
import LabelInput from './LabelInput';

function App() {
  let email = ''
  let code = ''
  const [showCode, setShowCode] = useState(false)
  const [content, setContent] = useState(
  <div className='wrapper'>
    <div className='register--container'>
      <LabelInput handleChange={e=>email = e.target.value} title='EMail'/>
      { showCode ? <LabelInput handleChange={e=>code = e.target.value} title='Code'/> : null }
      { showCode ? <CustomButton content='Подтвердить' onClick={
         ()=> {
           fetch('http://localhost:5050/register/activate', {
             method: 'POST',
             body: JSON.stringify({
               email: email,
               code: code
             }),
             headers: {
               'Content-type': 'application/json',
             }
           }).then((response) => {
             if(response.status == 200) {
               setContent(<h1>Успех!</h1>)
             }
             else {
               setContent(<h1>Неверный код!</h1>)
             }
           })
             .then((data) => console.log(data))
             .catch((err) => console.log(err.message));
             
         }
      }/> :  <CustomButton content='Отправить код' onClick={
        (e: any)=>
        {
          const body = JSON.stringify({
            email: email
          })          
          fetch('http://localhost:5050/register/new', {
            method: 'POST',
            body: body,
            headers: {
              'Content-type': 'application/json',
            }
          }).then((response) => response.json())
            .then((data) => console.log(data))
            .catch((err) => console.log(err.message));
          setShowCode(true)
        }
      }/>}
    </div>
  </div>);

  return (content);
}

export default App;
