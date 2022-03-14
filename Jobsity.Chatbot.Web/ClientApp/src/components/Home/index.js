import * as React from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { NavLink } from 'reactstrap';

function Home() {
  const login = useSelector((x) => x.login);

  return (
    <StyledHome>
      <h1>Hi {login.user.name}! 😁</h1>
      <h2>
        <NavLink tag={Link} className="text-dark" to="/rooms">Join to a chat room</NavLink>
      </h2>
    </StyledHome>
  );
}

const StyledHome = styled.div`
  a {
    color: blue !important;
  };
  a:hover {
    text-decoration: underline;
  }
`

export default Home;