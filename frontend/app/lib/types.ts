export interface BlogPost {
  id: number;
  title: string;
  body: string;
  createdAt: string;
}

export interface CreateBlogPostDto {
  title: string;
  body: string;
}

export interface UpdateBlogPostDto {
  title: string;
  body: string;
}