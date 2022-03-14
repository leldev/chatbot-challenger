import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useHistory } from 'react-router';
import styled from 'styled-components';
import { Button, Input, Spinner, Alert } from 'reactstrap';
import { actionCreators } from '../../store/Login';

function Register() {
  const [passwordValue, setPasswordValue] = useState('');
  const [nameValue, setNameValue] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const NAMEMAXLENGTH = 40;
  const PASSWORDMAXLENGTH = 50;

  const login = useSelector((x) => x.login);
  const dispatch = useDispatch();
  const history = useHistory();

  useEffect(() => {
  }, [])

  useEffect(() => {
    if (login.errorRegister) {
      setErrorMessage('Register error, try again.');
      setIsLoading(false);
    }
    if (login.isLoged) {
      history.push('/');
    }
  }, [login])


  const handlerOnNameChange = (e) => {
    setNameValue(e.target.value)
    setErrorMessage('');
  }

  const handlerOnPasswordChange = (e) => {
    setPasswordValue(e.target.value)
    setErrorMessage('');
  }

  const handlerOnClick = () => {
    if (nameValue && passwordValue) {
      dispatch(actionCreators.register(nameValue, passwordValue));
      setIsLoading(true);
    }
  }

  const renderRegister = () => {
    const nameProps = {
      type: 'text',
      id: 'txtRegisterNameId',
      placeholder: 'Name',
      required: true,
      value: nameValue,
      maxLength: NAMEMAXLENGTH,
      onChange: handlerOnNameChange
    };

    const passwordProps = {
      type: 'password',
      id: 'txtRegisterPasswordId',
      placeholder: 'Password',
      required: true,
      value: passwordValue,
      maxLength: PASSWORDMAXLENGTH,
      onChange: handlerOnPasswordChange
    };

    const loginButtonProps = {
      type: 'submit',
      id: 'btnRegisterId',
      disabled: isLoading,
      onClick: handlerOnClick
    }

    const loading = <Spinner animation="grow" size="sm" />;

    return (
      <StyledRegister>
        <Input {...nameProps} />
        <Input {...passwordProps} />
        {errorMessage && renderErrorMessage()}
        <Button {...loginButtonProps}>{isLoading && loading} Register</Button>
      </StyledRegister>
    )
  }

  const renderErrorMessage = () => {
    const errorMessageProps = {
      className: 'alert-danger',
    }

    return (<Alert {...errorMessageProps}>{errorMessage}</Alert>)
  }

  return (
    <div>
      {renderRegister()}
    </div>
  );
}

const StyledRegister = styled.div`
  .alert-danger,
  #txtRegisterPasswordId,
  #btnRegisterId {
    margin-top: 1rem;
  }
`

export default Register;