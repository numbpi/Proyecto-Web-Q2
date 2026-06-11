export interface ILoginResponse {
  token: string;
}

export interface IRigisterResponse {
  id: string;
  fullName: string;
  password: string;
  email: string;
  role: string;
}
