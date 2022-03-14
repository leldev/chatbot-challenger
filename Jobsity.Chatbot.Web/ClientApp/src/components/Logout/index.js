import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import styled from 'styled-components';
import { actionCreators } from '../../store/Login';

function Logout() {
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(actionCreators.logout())
  }, [])

  const renderLogout = () => {
    return (
      <h1>Logout</h1>
    );
  }

  return (
    <StyledLogin>
      {renderLogout()}
    </StyledLogin>
  );
}

const StyledLogin = styled.div`
`

export default Logout;