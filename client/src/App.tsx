import { useState } from 'react';
import './App.css';
import CustomButton from './CustomButton';
import LabelInput from './LabelInput';

function App() {
  const [showCode, setShowCode] = useState(false)
  const [btnMsg, setBtnMsg] = useState('Отправить код')
  const [email, setEMail] = useState('')

  return (
    <div className='wrapper'>
      <div className='register--container'>
        <LabelInput handleChange={e=>setEMail(e.target.value)} title='EMail'/>
        { showCode ? <LabelInput handleChange={e=>setEMail(e.target.value)} title='Code'/> : null }
        <CustomButton content={btnMsg} onClick={()=>
        {
          fetch('http://localhost:5050/register/new', {
            method: 'POST',
            body: JSON.stringify({
              email: email
            }),
            headers: {
              'Content-type': 'application/json',
            }
          }).then((response) => response.json())
            .then((data) => console.log(data))
            .catch((err) => console.log(err.message));
          setShowCode(true)
          setBtnMsg('Завершить')
        }}/>
      </div>
    </div>
  );
}

export default App;
