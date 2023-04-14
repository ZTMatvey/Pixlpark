import CustomButton from "./CustomButton"
import LabelInput from "./LabelInput"
import { useState } from 'react';
import { useNavigate } from "react-router-dom";

function Register() {
    let email = ''
    let code = ''
    const [showCode, setShowCode] = useState(false)
    const navigate = useNavigate(); 
    return (
        <><LabelInput handleChange={e=>email = e.target.value} title='EMail'/>
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
                navigate("/register/success")
               }
               else {
                navigate("/register/fail")
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
            setShowCode(true)
            fetch('http://localhost:5050/register/new', {
              method: 'POST',
              body: body,
              headers: {
                'Content-type': 'application/json',
              }
            }).then((response) => response.json())
              .then((data) => console.log(data))
              .catch((err) => console.log(err.message));
          }
        }/>}</>);
    }
    
    export default Register;
          