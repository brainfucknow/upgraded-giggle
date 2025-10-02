import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../lib/api';
import type { CreateBlogPostDto, UpdateBlogPostDto } from '../lib/types';

const QUERY_KEYS = {
  blogPosts: ['blogPosts'] as const,
  blogPost: (id: number) => ['blogPosts', id] as const,
};

export function useGetBlogPosts() {
  return useQuery({
    queryKey: QUERY_KEYS.blogPosts,
    queryFn: () => apiClient.getAllBlogPosts(),
  });
}

export function useGetBlogPost(id: number) {
  return useQuery({
    queryKey: QUERY_KEYS.blogPost(id),
    queryFn: () => apiClient.getBlogPostById(id),
    enabled: !!id,
  });
}

export function useCreateBlogPost() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (data: CreateBlogPostDto) => apiClient.createBlogPost(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: QUERY_KEYS.blogPosts });
    },
  });
}

export function useUpdateBlogPost() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: UpdateBlogPostDto }) => 
      apiClient.updateBlogPost(id, data),
    onSuccess: (_, { id }) => {
      queryClient.invalidateQueries({ queryKey: QUERY_KEYS.blogPosts });
      queryClient.invalidateQueries({ queryKey: QUERY_KEYS.blogPost(id) });
    },
  });
}

export function useDeleteBlogPost() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (id: number) => apiClient.deleteBlogPost(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: QUERY_KEYS.blogPosts });
    },
  });
}