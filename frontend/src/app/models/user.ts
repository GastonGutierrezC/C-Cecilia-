export interface UserData {
  username: string;
  email: string;
}

export interface UserModel {
  id: number;
  username: string;
  email: string;
}

export interface TokenData {
  Id: string
  Username: string;
  nbf: number;
  exp: number;
  iat: number;
}

export interface loginResponse {
  token: string
}
