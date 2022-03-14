import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useHistory } from 'react-router';
import styled from 'styled-components';
import { Button, Input, Spinner, Alert } from 'reactstrap';
import { actionCreators } from '../../store/Login';

function Login() {
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
    if (login.error) {
      setErrorMessage('Login error, try again.');
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
      dispatch(actionCreators.login(nameValue, passwordValue))
      setIsLoading(true);
    }
  }

  const renderLogin = () => {
    const nameProps = {
      type: 'text',
      id: 'txtLoginNameId',
      placeholder: 'Name',
      required: true,
      value: nameValue,
      maxLength: NAMEMAXLENGTH,
      onChange: handlerOnNameChange
    };

    const passwordProps = {
      type: 'password',
      id: 'txtLoginPasswordId',
      placeholder: 'Password',
      required: true,
      value: passwordValue,
      maxLength: PASSWORDMAXLENGTH,
      onChange: handlerOnPasswordChange
    };

    const loginButtonProps = {
      type: 'submit',
      id: 'btnLoginId',
      disabled: isLoading,
      onClick: handlerOnClick
    }

    const loading = <Spinner animation="grow" size="sm" />;

    return (
      <div>
        <Input {...nameProps} />
        <Input {...passwordProps} />
        {errorMessage && renderErrorMessage()}
        <Button {...loginButtonProps}>{isLoading && loading} Login</Button>
      </div>
    )
  }

  const renderErrorMessage = () => {
    const errorMessageProps = {
      className: 'alert-danger',
    }

    return (<Alert {...errorMessageProps}>{errorMessage}</Alert>)
  }

  return (
    <StyledLogin>
      {renderLogin()}
    </StyledLogin>
  );
}

const StyledLogin = styled.div`
  .alert-danger,
  #txtLoginPasswordId,
  #btnLoginId {
    margin-top: 1rem;
  }
`

export default Login;