import type { BlogPost, CreateBlogPostDto, UpdateBlogPostDto } from './types';

const API_BASE_URL = 'http://localhost:5196/api';

class ApiClient {
  private async fetch<T>(endpoint: string, options?: RequestInit): Promise<T> {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      headers: {
        'Content-Type': 'application/json',
        ...options?.headers,
      },
      ...options,
    });

    if (!response.ok) {
      throw new Error(`API Error: ${response.status} ${response.statusText}`);
    }

    return response.json();
  }

  async getAllBlogPosts(): Promise<BlogPost[]> {
    return this.fetch<BlogPost[]>('/blogposts');
  }

  async getBlogPostById(id: number): Promise<BlogPost> {
    return this.fetch<BlogPost>(`/blogposts/${id}`);
  }

  async createBlogPost(data: CreateBlogPostDto): Promise<BlogPost> {
    return this.fetch<BlogPost>('/blogposts', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  }

  async updateBlogPost(id: number, data: UpdateBlogPostDto): Promise<BlogPost> {
    return this.fetch<BlogPost>(`/blogposts/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  }

  async deleteBlogPost(id: number): Promise<void> {
    await fetch(`${API_BASE_URL}/blogposts/${id}`, {
      method: 'DELETE',
    });
  }
}

export const apiClient = new ApiClient();