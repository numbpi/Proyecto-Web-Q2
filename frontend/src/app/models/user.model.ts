export interface ILoginResponse {
  token: string;
}

export interface IRegisterResponse {
  id: string;
  fullName: string;
  password: string;
  email: string;
  role: string;
}
